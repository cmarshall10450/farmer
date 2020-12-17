[<AutoOpen>]
module Farmer.Arm.DataFactory

open Farmer
open Farmer.DataFactory

let factories = ResourceType ("Microsoft.DataFactory/factories", "2018-06-01")

type AccountName =
    | AccountName of string
    static member Empty = AccountName ""
    member this.Value =
        let (AccountName account) = this
        account

type RepoName =
    | RepoName of string
    static member Empty = RepoName ""
    member this.Value =
        let (RepoName repo) = this
        repo

type Repository =
    { Account: AccountName
      Repo:  RepoName }
    static member Create(account: AccountName, repo: RepoName) = { Account = account; Repo = repo }
    
type GitConfig =
    { Provider: GitProvider
      Repository: Repository
      CollaborationBranch: string
      RootFolder: string }

type DataFactory =
    { Name : ResourceName
      Location: Location
      GitConfiguration: GitConfig option 
      Tags: Map<string, string> }
    interface IArmResource with
        member this.ResourceId = accounts.resourceId this.Name
        member this.JsonModel =
            {| factories.Create(this.Name, this.Location, tags = this.Tags) with
                 properties =
                  {| repoConfiguration =
                        this.GitConfiguration
                        |> Option.map(fun config ->
                            {| ``type`` = sprintf "Factory%sConfiguration" (config.Provider.ToString())
                               accountName = config.Repository.Account.Value
                               repositoryName = config.Repository.Repo.Value
                               collaborationBranch = config.CollaborationBranch
                               rootFoler = config.RootFolder |} |> box)
                        |> Option.toObj |}
            |} :> _