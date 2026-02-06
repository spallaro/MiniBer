# MiniBer

Versatile **ASN.1** parser for .NET (BER/DER/CER/TLV).

## Overview

**MiniBer** is a developer-friendly library designed to parse and navigate **ASN.1** binary structures. It provides a straightforward way to handle Tag-Length-Value (TLV) data, supporting the most common encoding rules used in cryptography, telecommunications, and digital certificates.

## Features

* **Multi-Standard Support**: decode data encoded with **BER** (Basic), **DER** (Distinguished), and **CER** (Canonical) rules.
* **TLV Navigation**: simple API to traverse complex and nested ASN.1 trees.
* **Flexible Compatibility**: works with both modern `.NET 8.0+` applications and legacy `.NET Standard 2.0` projects.
* **AOT Ready**: works with projects published using AOT (Ahead-Of-Time), supports trimming.
* **Lightweight**: zero external dependencies.
