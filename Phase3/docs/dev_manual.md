# 🌐 Manual de Desarrollador: Estructuras Avanzadas en AUTOGEST PRO

## 📋 Tabla de Contenidos
1. [Introducción](#-introducción)
2. [Blockchain para Usuarios](#-blockchain-para-usuarios)
   - [Estructura](#estructura-blockchain)
   - [Operaciones clave](#operaciones-clave-blockchain)
3. [Árbol de Merkle](#-árbol-de-merkle)
   - [Estructura](#estructura-merkle)
   - [Operaciones clave](#operaciones-clave-merkle)
4. [Grafo No Dirigido](#-grafo-no-dirigido)
   - [Estructura](#estructura-grafo)
   - [Operaciones clave](#operaciones-clave-grafo)
5. [Compresión Huffman](#-compresión-huffman)
   - [Estructura](#estructura-huffman)
   - [Operaciones clave](#operaciones-clave-huffman)
6. [Visualización](#-visualización)
7. [Benchmarking](#-benchmarking)
8. [Mejores Prácticas](#-mejores-prácticas)

---

## 🌟 Introducción
Implementación de estructuras avanzadas en C# para el sistema de gestión automotriz AUTOGEST PRO:

```mermaid
graph LR
    S[Sistema] --> B[Blockchain: Usuarios]
    S --> M[Merkle: Facturas]
    S --> G[Grafo: Vehículos-Repuestos]
    S --> H[Huffman: Backups]
```

---

## ⛓️ Blockchain para Usuarios

### Estructura Blockchain
```csharp
public class Block {
    public int Index { get; set; }
    public string Timestamp { get; set; }
    public string Data { get; set; }  // Serializado JSON del usuario
    public int Nonce { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    [JsonIgnore]
    public Block Next { get; set; }
}
```

### Operaciones Clave Blockchain
| Método | Complejidad | Descripción |
|--------|------------|-------------|
| `CalculateHash()` | O(1) | Genera hash SHA-256 del bloque |
| `MineBlock()` | O(n) | Prueba de trabajo (PoW) con nonce |
| `AddBlock()` | O(n) | Añade bloque validando hash anterior |
| `ValidateCredentials()` | O(n) | Verifica usuario con hash SHA-256 |

**Flujo de Minería**:
```mermaid
graph TB
    A[Iniciar Minería] --> B{Hash comienza con 0000?}
    B -->|No| C[Incrementar Nonce]
    B -->|Sí| D[Bloque Minado]
```

---

## 🌿 Árbol de Merkle

### Estructura Merkle
```csharp
public class MerkleNode {
    public Bill Factura { get; set; }
    public string Hash { get; set; }
    public MerkleNode Left { get; set; }
    public MerkleNode Right { get; set; }
}
```

### Operaciones Clave Merkle
| Método | Complejidad | Descripción |
|--------|------------|-------------|
| `BuildTree()` | O(n log n) | Construye árbol desde hojas |
| `GenerateHash()` | O(1) | Combina hashes hijos |
| `GetBillsByServiceIds()` | O(n) | Búsqueda por IDs de servicio |

**Ejemplo de Construcción**:
```mermaid
graph BT
    A[Hash1] --> C[Hash1+2]
    B[Hash2] --> C
    D[Hash3] --> E[Hash3+4]
    F[Hash4] --> E
    C --> G[Root]
    E --> G
```

---

## 🕸️ Grafo No Dirigido

### Estructura Grafo
```csharp
public class UnDirectedGraph {
    private Dictionary<string, List<string>> listaAdyacencia;
    // Key: ID de vehículo/repuesto
    // Value: Lista de conexiones
}
```

### Operaciones Clave Grafo
| Método | Complejidad | Descripción |
|--------|------------|-------------|
| `Insertar()` | O(1) | Añade relación bidireccional |
| `GenerarDot()` | O(V+E) | Exporta para Graphviz |

**Relación Vehículo-Repuesto**:
```mermaid
graph LR
    V1[Vehículo 101] -- Usa --> R1[Repuesto 201]
    V1 -- Usa --> R2[Repuesto 202]
    R1 -- Instalado en --> V1
```

---

## 📦 Compresión Huffman

### Estructura Huffman
```csharp
public class HuffmanNode {
    public char Character { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }
}
```

### Operaciones Clave Huffman
| Método | Complejidad | Descripción |
|--------|------------|-------------|
| `BuildHuffmanTree()` | O(n log n) | Construye árbol de frecuencias |
| `Compress()` | O(n) | Codifica texto con tabla Huffman |
| `Decompress()` | O(n) | Decodifica usando árbol |

**Proceso de Compresión**:
```mermaid
graph LR
    A[Texto Original] --> B[Conteo Frecuencias]
    B --> C[Árbol Huffman]
    C --> D[Tabla Códigos]
    D --> E[Texto Binario]
```

---

## 📊 Visualización
Todas las estructuras implementan:

```csharp
public string GenerateDot() {
    // Genera código DOT para Graphviz
}
```

**Ejemplo Blockchain**:
```dot
digraph Blockchain {
    node [shape=record];
    "Bloque 0" [label="Index: 0|Hash: abc123..."];
    "Bloque 1" [label="Index: 1|Hash: def456..."];
    "Bloque 0" -> "Bloque 1";
}
```

---

## ⚡ Benchmarking
| Estructura | Inserción | Búsqueda | Memoria | Caso Ideal |
|------------|----------|----------|---------|------------|
| Blockchain | O(n) | O(n) | Alta | Registros inmutables |
| Merkle | O(log n) | O(n) | Media | Verificación integridad |
| Grafo | O(1) | O(V+E) | Variable | Relaciones complejas |
| Huffman | O(n log n) | O(n) | Baja | Compresión texto |

---

## 🏆 Mejores Prácticas

**Blockchain**
```diff
+ Ideal para auditoría de usuarios
+ Inmutabilidad garantizada
- No para datos volátiles
```

**Árbol Merkle**
```diff
+ Perfecto para facturas
+ Verificación rápida de integridad
```

**Grafo No Dirigido**
```diff
+ Modela relaciones vehículo-repuesto
+ Búsqueda bidireccional
```

**Huffman**
```diff
+ Máxima compresión para backups
+ Eficiente con datos repetitivos
```

---

## 📝 Conclusión
```mermaid
pie
    title Uso Recomendado
    "Blockchain" : 30
    "Merkle" : 25
    "Grafo" : 25
    "Huffman" : 20
```
**Criterios de Selección**:
1. **Persistencia**: Blockchain para datos críticos
2. **Integridad**: Merkle para facturas
3. **Relaciones**: Grafo para conexiones complejas
4. **Compresión**: Huffman para backups grandes