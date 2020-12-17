[<AutoOpen>]
module Farmer.Builders.DataFactory

open Farmer
open Farmer.DataFactory
open Farmer.Arm.DataFactory

type DataFactoryConfig =
    { Name: ResourceName
      GitConfiguration: GitConfig option
      Tags: Map<string, string>}
    interface IBuilder with
        member this.ResourceId = factories.resourceId this.Name
        member this.BuildResources location = [
            { Name = this.Name
              Location = location
              GitConfiguration = this.GitConfiguration 
              Tags = this.Tags }
        ]

type DataFactoryBuilder() =
    member __.Yield _ =
        { Name = ResourceName ""
          GitConfiguration = None
          Tags = Map.empty}
    /// Sets the name of the data lake.
    [<CustomOperation "name">]
    member _.Name (state:DataFactoryConfig, name) =
        { state with Name = ResourceName name }
    [<CustomOperation "git_provider">]
    member _.GitProvider(state:DataFactoryConfig, provider) =
        let gitConfig =
            match state.GitConfiguration with
            | Some _ ->
                state.GitConfiguration
                |> Option.map (fun config -> {config with Provider = provider })
            | None ->
                Some({ Provider = provider
                       Repository = Repository.Create(AccountName.Empty, RepoName.Empty)
                       CollaborationBranch = "main"
                       RootFolder = "/" })
        { state with GitConfiguration = gitConfig }
    [<CustomOperation "git_repository">]
    member _.GitRepository(state: DataFactoryConfig, account: AccountName, repoName: RepoName) =
        let repository = Repository.Create(account, repoName)
        let gitConfig =
            match state.GitConfiguration with
            | Some _ ->
                state.GitConfiguration
                |> Option.map (fun config -> {config with Repository = repository })
            | None ->
                Some({ Provider = GitHub
                       Repository = repository
                       CollaborationBranch = "main"
                       RootFolder = "/" })
        {state with GitConfiguration = gitConfig}
    member this.GitRepository(state: DataFactoryConfig, account, repo) =
        this.GitRepository(state, AccountName account, RepoName repo)
    [<CustomOperation "git_branch">]
    member _.GitBranch(state: DataFactoryConfig, branch) =
        let gitConfig =
            match state.GitConfiguration with
            | Some _ ->
                state.GitConfiguration
                |> Option.map (fun config -> {config with CollaborationBranch = branch })
            | None ->
                Some({ Provider = GitHub
                       Repository = Repository.Create(AccountName.Empty, RepoName.Empty)
                       CollaborationBranch = branch
                       RootFolder = "/" })
        {state with GitConfiguration = gitConfig}
    [<CustomOperation "git_root_folder">]
    member _.GitRootFolder(state: DataFactoryConfig, folder) =
        let gitConfig =
            match state.GitConfiguration with
            | Some _ ->
                state.GitConfiguration
                |> Option.map (fun config -> {config with RootFolder = folder })
            | None ->
                Some({ Provider = GitHub
                       Repository = Repository.Create(AccountName.Empty, RepoName.Empty)
                       CollaborationBranch = "main"
                       RootFolder = folder })
        {state with GitConfiguration = gitConfig}
    [<CustomOperation "add_tags">]
    member _.Tags(state: DataFactoryConfig, pairs) =
        { state with
            Tags = pairs |> List.fold (fun map (key,value) -> Map.add key value map) state.Tags }
    [<CustomOperation "add_tag">]
    member this.Tag(state: DataFactoryConfig, key, value) = this.Tags(state, [ (key,value) ])

let factory = DataFactoryBuilder()