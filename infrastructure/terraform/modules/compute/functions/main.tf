resource "azurerm_storage_account" "storage_accounts" {
  for_each = { for f in var.all_functions : f.name => f }

  name                     = replace("${each.value.name}", "-", "")
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_nested_items_to_be_public = false

  tags = {
    source = "Functions"
  }
}

resource "azurerm_linux_function_app" "apps" {
    for_each = { for f in var.all_functions : f.name => f }

  name                        = each.value.name
  resource_group_name         = var.resource_group_name
  location                    = var.location
  service_plan_id             = var.service_plan_id

  storage_account_name        = azurerm_storage_account.storage_accounts[each.value.name].name
  storage_account_access_key  = azurerm_storage_account.storage_accounts[each.value.name].primary_access_key

  site_config {
    application_stack {
      dotnet_version = each.value.dotnet_version
      use_dotnet_isolated_runtime = true
    }
  }
}