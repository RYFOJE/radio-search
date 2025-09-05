resource "azurerm_storage_account" "storage_accounts" {
  name                     = lower(replace("${var.service_plan_name}-${random_string.suffix.result}", "-", ""))
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_nested_items_to_be_public = false

  tags = {
    source = "Functions"
  }
}

resource "azurerm_storage_container" "containers" {
  for_each = { for f in var.all_functions : f.name => f }

  name                  = replace("${each.value.name}", "-", "")
  storage_account_id    = azurerm_storage_account.storage_accounts.id
  container_access_type = "private"
}


resource "azurerm_service_plan" "service_plan" {
  name                = var.service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = "FC1"
}

resource "azurerm_function_app_flex_consumption" "apps" {
  for_each = { for f in var.all_functions : f.name => f }

  name                = each.value.name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.service_plan.id
  runtime_name        = each.value.runtime_name
  runtime_version     = each.value.runtime_version

  # Storage configuration
  storage_container_type      = "blobContainer"
  storage_container_endpoint  = "${azurerm_storage_account.storage_accounts.primary_blob_endpoint}${azurerm_storage_container.containers[each.value.name].name}"
  storage_authentication_type = "StorageAccountConnectionString"
  storage_access_key          = azurerm_storage_account.storage_accounts.primary_access_key

  site_config {
  }
}

resource "random_string" "suffix" {
  length  = 6
  upper   = false
  special = false
}