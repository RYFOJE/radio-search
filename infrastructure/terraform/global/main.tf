provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

# Resource Group for Terraform state
resource "azurerm_resource_group" "tfstate" {
  name     = var.resource_group_name
  location = var.location
}

# Storage Account for Terraform state
resource "azurerm_storage_account" "tfstate" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.tfstate.name
  location                 = azurerm_resource_group.tfstate.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  public_network_access_enabled = true
  allow_nested_items_to_be_public = false
  access_tier = "Cold"
}

# Blob container for Terraform state files
resource "azurerm_storage_container" "tfstate" {
  name                  = "tfstate"
  storage_account_id    = azurerm_storage_account.tfstate.id
  container_access_type = "private"
}
