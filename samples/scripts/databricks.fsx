#r @"./libs/Newtonsoft.Json.dll"
#r @"../../src/Farmer/bin/Debug/netstandard2.0/Farmer.dll"

open Farmer
open Farmer.Builders
open Farmer.Databricks

let workspace = databricksWorkspace {
    name "my-databricks-workspace"
    sku Databricks.Sku.Standard
    
    use_public_ip Enabled
    
    key_vault "databricks-kv"
    encryption_key "workspace-encryption-key" "latest"
    
    byov_vnet "databricks-vnet"
    byov_public_subnet "databricks-pub-snet"
    byov_private_subnet "databricks-priv-snet"
}

let deployment = arm {
    location Location.NorthEurope
    add_resource workspace
}

// Generate the ARM template here...
deployment
|> Writer.quickWrite "farmer-deploy"