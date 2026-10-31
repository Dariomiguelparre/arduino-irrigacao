# Simulador (sem Arduino físico)

Teste a lógica do sistema no Mac **sem** sensor, LED ou placa.

O simulador em **C#** usa as mesmas regras do firmware:

- leitura a cada **100 ms**
- limiar de **40%**
- histerese de **3%**
- LED “aceso” quando precisa regar

## Executável pronto

Na pasta do projeto:

```bash
./dist/IrrigacaoSimulador
```

Ou, pelo caminho completo:

```bash
/Users/joaovitorcamargo/www/arduino_irrigacao/dist/IrrigacaoSimulador
```

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

## Rodar pelo código-fonte

Se quiser alterar e testar de novo:

```bash
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$PATH"

cd /Users/joaovitorcamargo/www/arduino_irrigacao/simulador/IrrigacaoSimulador
dotnet run
```

## Gerar o executável de novo

```bash
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$PATH"

cd /Users/joaovitorcamargo/www/arduino_irrigacao/simulador/IrrigacaoSimulador
dotnet publish -c Release -r osx-arm64 --self-contained true \
  -p:PublishSingleFile=true \
  -o ../../dist
```

O binário fica em `dist/IrrigacaoSimulador`.
