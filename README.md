# APARTMENTS <!-- omit in toc -->

VUA, RWA project for digitalization of apartment management system.

## Table of contents <!-- omit in toc -->
- [1. Description](#1-description)
- [2. Installation](#2-installation)
- [3. Authors](#3-authors)
- [4. License](#4-license)

## 1. Description

APARTMENTS is a CRUD application system designed for managing apartments. Application consists of two separate applications: Web Forms application for administration of apartments, and MVC application for public preview of available apartments.

## 2. Installation

Before running application user must publish and deploy database located in **Apartments.DB** project. After database is published, check and update connection string located in **Apartments.AdminUI** and **Apartments.WebUI** projects. Default connection string is targeting **default instance**, database with name **APARTMENTS**, and user **sa** with password **SQL**.

After database is configured, user can build and run both applications.

## 3. Authors

- **Karlo Bratko** (karlo.bratko@racunarstvo.hr)

## 4. License

This project is licensed under the **MIT** license - see the [LICENSE.md](/LICENSE.md) file for details.
