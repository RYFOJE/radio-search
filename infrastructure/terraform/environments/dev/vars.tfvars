location = "Canada Central"
resource_group_name = "Radio-Search"

consumption_plans = {
  plan1 = {
    name  = "flex-consumption"
    sku   = "Y1"
    all_functions = [
      {
        name           = "importer-canada"
        runtime_name   = "dotnet-isolated"
        runtime_version = "8.0"
      }
    ]
  }
}