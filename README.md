# MiniBer

Versatile **ASN.1** parser for .NET (BER/DER/CER/TLV).

[![NuGet version](https://img.shields.io/nuget/v/MiniBer.svg)](https://www.nuget.org/packages/MiniBer/)
![Target Framework](https://img.shields.io/badge/.NET-8.0%20%7C%20Standard%202.0-white)
[![License: MIT](https://img.shields.io/badge/License-MIT-white.svg)](https://opensource.org/licenses/MIT)

## Overview

**MiniBer** is a developer-friendly library designed to parse and navigate **ASN.1** binary structures. It provides a straightforward way to handle Tag-Length-Value (TLV) data, supporting the most common encoding rules used in cryptography, telecommunications, and digital certificates.

## Features

* **Multi-Standard Support**: decode data encoded with **BER** (Basic), **DER** (Distinguished), and **CER** (Canonical) rules.
* **TLV Navigation**: simple API to traverse complex and nested ASN.1 trees.
* **Flexible Compatibility**: works with both modern `.NET 8.0+` applications and legacy `.NET Standard 2.0` projects.
* **AOT Ready**: works with projects published using AOT (Ahead-Of-Time), supports trimming.
* **Lightweight**: zero external dependencies.

## Installation

Install via NuGet:

```bash
dotnet add package MiniBer
```

## Usage

### Quick start: decoding data
```csharp
byte[] data = [
    0x01, 0x01, 0xFF,                        // ELEMENT #1
    0x9F, 0x1A, 0x02, 0x12, 0x34,            // ELEMENT #2
    0x5C, 0x00,                              // ELEMENT #3
    0xDF, 0x02, 0x81, 0x02, 0xAB, 0xCD,      // ELEMENT #4
    0xE1, 0x05, 0x30, 0x03, 0x02, 0x01, 0x01 // ELEMENT #5
];

MiniBer.Nodes nodes = MiniBer.Decoder.Decode(
    data: data);
```

Parsed nodes:
| TagNumber | IdentifierOctets | Class           | ContentType | Length | Contents                     |
|-----------|------------------|-----------------|-------------|--------|------------------------------|
| 0x01      | 0x01             | Universal       | Primitive   | 1      | 0xFF                         |
| 0x1A      | 0x9F, 0x1A       | ContextSpecific | Primitive   | 2      | 0x12, 0x34                   |
| 0x1C      | 0x5C             | Application     | Primitive   | 0      |                              |
| 0x02      | 0xDF, 0x02       | Private         | Primitive   | 2      | 0xAB, 0xCD                   |
| 0x01      | 0xE1             | Private         | Constructed | 5      | 0x30, 0x03, 0x02, 0x01, 0x01 |

### Searching nodes by TagNumber

Searches a `Nodes` objects, looking for nodes with a specific `TagNumber` property.

```csharp
MiniBer.Nodes foundNodes = nodes.Search(
    tagNumber: 0x01);
```
Found nodes:
| TagNumber | IdentifierOctets | Class           | ContentType | Length | Contents                     |
|-----------|------------------|-----------------|-------------|--------|------------------------------|
| 0x01      | 0x01             | Universal       | Primitive   | 1      | 0xFF                         |
| 0x01      | 0xE1             | Private         | Constructed | 5      | 0x30, 0x03, 0x02, 0x01, 0x01 |

### Searching nodes by IdentifierOctects

Searches a `Nodes` objects, looking for nodes with a specific `TagNumber` property.

```csharp
MiniBer.Nodes foundNodes1 = nodes.Search(
    identifierOctects: [0xDF, 0x02]);
```

Or:

```csharp
MiniBer.Nodes foundNodes2 = nodes[0xDF, 0x02];
```

Found node:
| TagNumber | IdentifierOctets | Class           | ContentType | Length | Contents                     |
|-----------|------------------|-----------------|-------------|--------|------------------------------|
| 0x02      | 0xDF, 0x02       | Private         | Primitive   | 2      | 0xAB, 0xCD                   |

### Looking for a subnode, knowing it's index

```csharp
MiniBer.Node? node = nodes.SearchPath(4, 0, 0);
```

Or:

```csharp
MiniBer.Node node2 = nodes[4].Nodes[0].Nodes[0];
```

Found node:
| TagNumber | IdentifierOctets | Class           | ContentType | Length | Contents                     |
|-----------|------------------|-----------------|-------------|--------|------------------------------|
| 0x02      | 0x02             | Universal       | Primitive   | 1      | 0x01                         |


## API Reference

### Object MiniBer.Decoder
| Method | Return type | Description |
|--------|-------------|-------------|
| `Decode(byte[] data, DecodeOptions decodeOptions)` | `Nodes` | Decodes `data`, specifing decoding options. |
| `Decode(byte[] data)`                              | `Nodes` | Same as `Decode(data, DecodeOptions.None)`. |

### Object MiniBer.Nodes
| Method | Return type | Description |
|--------|-------------|-------------|
| `Search(int tagNumber)`                 | `Nodes` | Search all nodes based on `TagNumber` property. |
| `Search(byte[] identifierOctects)`      | `Nodes` | Search all nodes, based on `IdentifierOctects` property. |
| `this[params byte[] identifierOctects]` | `Nodes` | Same as `Search(byte[] identifierOctects)`. |
| `SearchPath(params int[] path)`         | `Node?` | Searches a node on a path of indexes. |

### Object MiniBer.Node
| Property | Type | Description |
|----------|------|-------------|
| `TagNumber`          | `int`          | The elaborated tag number. |
| `IdentifierOctets`   | `List<byte>?`  | The original identifier octects. |
| `Class`              | `Classes`      | Class of the node. |
| `ContentType`        | `ContentTypes` | Content type of the node. |
| `Nodes`              | `Nodes?`       | Inner nodes of the node. |
| `Length`             | `int`          | The calculated length of the contents. |
| `IndefinitiveLength` | `bool`         | Determines if `Lenght` is indefinitive. |
| `LengthOctects`      | `List<byte>?`  | The original length octects. |
| `Contents`           | `byte[]?`      | Data contents. |

| Method | Return type | Description |
|--------|-------------|-------------|
| `TryParseSubNodes(DecodeOptions decodeOptions)` | `bool` | Try parse `Contents` property to the `Nodes` property. |
| `TryParseSubNodes()` | `bool` | Same as `TryParseSubNodes(decodeOptions: DecodeOptions.None)`. |