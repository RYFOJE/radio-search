variable "subscription_id" {
  type    = string
  default = "e864341d-9146-443d-a6bb-5b47e11c828a"
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

variable "consumption_plans" {
  type = map(object({
    name         = string
    sku          = string
    all_functions = optional(list(object({
      name           = string
      runtime_name   = string
      runtime_version= string
    })), [])
  }))
  default = {}
}

variable "app_service_plans" {
  type = map(object({
    name      = string
    sku       = string
    functions = optional(list(object({
      name           = string
      dotnet_version = string
    })), [])
    app_service = optional(list(object({
      name = string
    })), [])
  }))
  default = {}
}
