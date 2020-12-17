---
title: "Data Factory"
date: 2020-12-17T20:14:00+00:00
chapter: false
weight: 8
---

#### Overview
The Data Factory builder is used to create Azure Data Factory instances.

* Data Factory (`Microsoft.DataFactory/factories`)

#### Builder Keywords
| Keyword | Purpose |
|-|-|
| name | Sets the name of the Data Factory instance. |
| git_provider | Sets the git provieder. Defaults to GitHub. Options: GitHub | DevOps |
| git_repository | Sets the repository account and name. |
| git_branch | Sets the collaboration branch. Defaults to 'main' |
| git_root_folder | Sets the root folder for where Data Factory resources are stored. Defaults to '/' |

#### Example
```fsharp
open Farmer
open Farmer.Builders

let myDataFactory = factory {
    name "myDataFactory"
    
    git_provider DataFactory.GitProvider.GitHub
    git_repository "myGitHubAccount" "myGitHubRepo"
    git_branch "main"
    git_root_folder "/"
}
```