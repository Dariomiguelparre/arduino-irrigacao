# Simulador (sem Arduino físico)

Teste a lógica do sistema no computador **sem** sensor, LED ou placa.

O simulador em **C#** usa as mesmas regras do firmware:

- leitura a cada **100 ms**
- limiar de **40%**
- histerese de **3%**
- LED “aceso” quando precisa regar

O executável **não vem no repositório** (é gerado na sua máquina). Depois de clonar, instale o .NET SDK e publique o binário.

## Pré-requisito: .NET SDK 8

Confira se já tem:

```bash
dotnet --version
```

Se não tiver, instale o **.NET 8 SDK**:

- macOS / Windows / Linux: <https://dotnet.microsoft.com/download/dotnet/8.0>

No macOS com Homebrew:

```bash
brew install --cask dotnet-sdk
```

## Opção A — Rodar direto do código (rápido)

Na raiz do projeto clonado:

```bash
cd simulador/IrrigacaoSimulador
dotnet run
```

Útil para desenvolver e testar alterações.

## Opção B — Gerar o executável

Gera um arquivo único em `dist/`, igual ao que existia localmente antes.

### 1. Escolha o runtime da sua máquina

| Sistema | Runtime (`-r`) |
|---------|----------------|
| macOS Apple Silicon (M1/M2/M3/M4) | `osx-arm64` |
| macOS Intel | `osx-x64` |
| Windows 64 bits | `win-x64` |
| Linux 64 bits | `linux-x64` |

### 2. Publique

Na raiz do projeto:

```bash
cd simulador/IrrigacaoSimulador

dotnet publish -c Release -r osx-arm64 --self-contained true \
  -p:PublishSingleFile=true \
  -o ../../dist
```

Troque `osx-arm64` pelo runtime da tabela se for outra plataforma.

O binário fica em:

- macOS / Linux: `dist/IrrigacaoSimulador`
- Windows: `dist/IrrigacaoSimulador.exe`

### 3. Execute

```bash
# macOS / Linux
./dist/IrrigacaoSimulador

# Windows (PowerShell ou cmd, a partir da raiz do projeto)
.\dist\IrrigacaoSimulador.exe
```

`--self-contained true` embute o runtime .NET no executável: quem receber só o arquivo de `dist/` **não precisa** instalar o SDK para rodar.

## Controles

| Tecla | Ação |
|-------|------|
| `↑` ou `+` | Aumentar umidade (+5%) |
| `↓` ou `-` | Diminuir umidade (−5%) |
| `R` | Simular rega (+25%) — “abrir o registro” |
| `A` | Ligar/desligar modo automático (solo seca sozinho) |
| `Q` ou `Esc` | Sair |

## O que observar

1. Com o modo **AUTOMÁTICO**, a umidade cai sozinha.
2. Quando passa **abaixo de 40%**, o LED fica **ACESO** e o alerta vira `REGAR`.
3. Pressione **R** para simular a rega: a umidade sobe e o LED apaga (com histerese, a partir de 43%).

## Código-fonte

- Projeto: [`../simulador/IrrigacaoSimulador/`](../simulador/IrrigacaoSimulador/)
- Lógica espelhada do firmware: [`../irrigacao/irrigacao.ino`](../irrigacao/irrigacao.ino)
