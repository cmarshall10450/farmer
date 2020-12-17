module DataFactory

open Expecto
open Farmer.Builders
open Farmer.DataFactory
open System
open Farmer

let tests = testList "DataFactory" [
    test "Should initialise correctly with basic configuration" {
        let dataFactory = factory {
            name "my-data-factory"
        }

        Expect.equal dataFactory.Name.Value "my-data-factory" "Name is not set correctly"
        Expect.equal dataFactory.GitConfiguration None "GitConfiguration should be None"
    }

    test "Should set git provider and initialise git configuration" {
        let dataFactory = factory {
            name "my-data-factory"
            git_provider DataFactory.GitProvider.DevOps
        }

        Expect.equal dataFactory.GitConfiguration.Value.Provider DataFactory.GitProvider.DevOps "Provider not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.Repository.Account.Value "" "Account not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.Repository.Repo.Value "" "Repo not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.CollaborationBranch "main" "Collaboration branch not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.RootFolder "/" "Root folder not set correctly"
    }

    test "Should set git configuration" {
        let dataFactory = factory {
            name "my-data-factory"
            git_provider DataFactory.GitProvider.GitHub
            git_repository "my-github-account" "my-github-repo"
            git_branch "not-main"
            git_root_folder "/not/root/folder"
        }

        Expect.equal dataFactory.GitConfiguration.Value.Provider DataFactory.GitProvider.GitHub "Provider not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.Repository.Account.Value "my-github-account" "Account not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.Repository.Repo.Value "my-github-repo" "Repo not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.CollaborationBranch "not-main" "Branch not set correctly"
        Expect.equal dataFactory.GitConfiguration.Value.RootFolder "/not/root/folder"
    }
]
