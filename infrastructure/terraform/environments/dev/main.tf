terraform {
  backend "azurerm" {
    resource_group_name   = var.backend_resource_group_name
    storage_account_name  = var.backend_storage_account_name
    container_name        = var.backend_container_name
    key                   = var.backend_key
  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

module "compute_app_service_plan" {
  source  = "../../modules/compute/app-service-plans"
  resource_group_name = azurerm_resource_group.rg.name
  location            = var.location 

  name    = var.compute_app_service_plan_name
  sku_name = var.compute_app_service_plan_sku_name
}