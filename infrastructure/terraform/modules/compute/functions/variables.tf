variable "location" {
  type = string
  default = "Canada Central"
}

variable "resource_group_name" {
  type = string
}

variable "service_plan_id" {
  type = string
}

variable "all_functions" {
  description = "List of all azure functions for a single app service plan"
  type = list(object({
    name = string
    dotnet_version = string
  }))
}
