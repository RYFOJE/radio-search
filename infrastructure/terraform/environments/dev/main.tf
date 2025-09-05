provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

module "consumption_apps" {
  for_each = { for f in var.consumption_plans : f.name => f }

  source = "../../modules/compute/flex_function_group"
  location = var.location
  resource_group_name = var.resource_group_name
  service_plan_name = each.value.name
  all_functions = each.value.all_functions
}