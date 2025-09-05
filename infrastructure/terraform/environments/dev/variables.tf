# BACKEND
variable "backend_storage_account_name" {
  type    = string
  default = "rdsterraform" 
}

variable "backend_resource_group_name" {
  type    = string
  default = "terraform"
}

variable "backend_container_name" {
  type    = string
  default = "tfstate"
}

variable "backend_key" {
  type    = string
  default = "terraform.tfstate"
}


# Actual values

variable "subscription_id" {
  type    = string
  default = "rdsimporter"
}

variable "resource_group_name" {
  type    = string
  default = "rdsimporter"
}

variable "location" {
  type    = string
  default = "Canada Central"
}

# COMPUTE

variable "compute_app_service_plan_name" {
  type    = string
  default = "compute-service-plan"
}

variable "compute_app_service_plan_sku_name" {
  type    = string
  default = "B1"
}

