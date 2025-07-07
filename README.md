# Number Conversion Application

A .NET 8 Blazor Server application that converts numeric dollar amounts into written English text.

## What it does

Converts numbers like `123.45` into `ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS`.

## Features

- Converts numbers up to 999 trillion (15 digits)
- Supports decimal amounts (cents)
- Real-time input validation
- Clean web interface

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

Check if you have .NET installed:
```bash
dotnet --version
```

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/prindas84/tech-one.git
cd tech-one
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Run the application
```bash
dotnet run --project NumberConvertion
```

### 4. Run tests
```bash
dotnet test
```

## Project Structure

```
tech-one/
├── NumberConvertion/          # Main application
├── NumberConversion.Tests/    # Unit tests
├── NumberConvertion.sln       # Solution file
└── README.md
```

## Usage

1. Enter a dollar amount (e.g., `123.45`)
2. View the converted text automatically
3. Valid formats:
   - Whole numbers: `123`
   - Decimals: `123.45`
   - Maximum: 15 digits before decimal

## Examples

| Input | Output |
|-------|--------|
| `1` | `ONE DOLLAR` |
| `25` | `TWENTY-FIVE DOLLARS` |
| `1.01` | `ONE DOLLAR AND ONE CENT` |
| `123.45` | `ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS` |

## Development

### Build the solution
```bash
dotnet build
```

### Run tests
```bash
dotnet test
```

## Troubleshooting

**Tests failing?**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet test
```

---

Built with .NET 8 and Blazor Server