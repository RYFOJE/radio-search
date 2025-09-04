terraform {
  backend "azurerm" {
    resource_group_name   = "terraform"
    storage_account_name  = "rdsterraform"
    container_name        = "tfstate"
    key                   = "dev.terraform.tfstate"
  }
}