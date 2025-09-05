variable "resource_group_name" {
  type = string
}

variable "location" {
  type    = string
  default = "Canada Central"
}

variable "sku_name" {
  type    = string
}

variable "name" {
  type    = string
}

variable "os_type" {
  type    = string
  default = "Linux"
}