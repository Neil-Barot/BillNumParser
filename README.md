# BillNumParser

Parses and validates bill numbers from strings.

## Installation

```bash
# Clone repository
git clone https://github.com/Neil-Barot/BillNumParser.git

# Navigate into folder
cd BillNumParser

# Restore dependencies
dotnet restore

# Build
dotnet build
```

## Public Methods & Properties

### Methods

#### `BillNumberLong()`
Formats the bill number into it's long form: ex. hr 0205 -> HR00205

**Returns**
- Long form of bill number `String`.

#### `BillNumberShort()`
Formats the bill number into it's short form: ex. hr 0205 -> HR 205

**Returns**
- Short form of bill number `String`.

---

### Properties

#### `IsValid` *(boolean, read-only)*
Gets whether the bill number is valid.

**Example**
```csharp
BillNumber example = new BillNumber("hb 0034");
Console.writeline(example.getIsValid); // 'true'
