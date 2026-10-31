# Instalação e gravação do firmware

## 1. Pré-requisitos neste Mac

O ambiente deste projeto usa:

```bash
export PATH="$HOME/.local/bin:$PATH"
arduino-cli version
```

Core AVR (Uno/Nano):

```bash
arduino-cli core list
# deve listar arduino:avr
```

Se precisar reinstalar:

```bash
mkdir -p "$HOME/.local/bin"
curl -fsSL https://raw.githubusercontent.com/arduino/arduino-cli/master/install.sh | BINDIR="$HOME/.local/bin" sh
export PATH="$HOME/.local/bin:$PATH"
arduino-cli config init --overwrite
arduino-cli core update-index
arduino-cli core install arduino:avr
```

## 2. Abrir o projeto

```bash
cd /Users/joaovitorcamargo/www/arduino_irrigacao
```

O sketch fica em `irrigacao/irrigacao.ino`.

## 3. Compilar (sem placa)

```bash
export PATH="$HOME/.local/bin:$PATH"
arduino-cli compile --fqbn arduino:avr:uno irrigacao
```

Para Nano clássico:

```bash
arduino-cli compile --fqbn arduino:avr:nano irrigacao
```

## 4. Conectar a placa e gravar

Liste as portas:

```bash
arduino-cli board list
```

Exemplo de gravação no Uno (ajuste a porta):

```bash
arduino-cli upload -p /dev/cu.usbmodem14101 --fqbn arduino:avr:uno irrigacao
```

No Mac, a porta costuma ser `/dev/cu.usbmodemXXXX` ou `/dev/cu.wchusbserialXXXX` (clone CH340).

## 5. Monitor serial

```bash
arduino-cli monitor -p /dev/cu.usbmodem14101 -c baudrate=9600
```

Você verá colunas: valor bruto, umidade %, e `REGAR` ou `OK`.

## 6. Ajustar limiar e pinos

No arquivo `irrigacao/irrigacao.ino`:

| Constante | Padrão | Significado |
|-----------|--------|-------------|
| `PIN_SENSOR` | `A0` | Pino do sensor |
| `PIN_LED_VERMELHO` | `8` | Pino do LED |
| `INTERVALO_LEITURA_MS` | `100` | Taxa de amostragem |
| `LIMIAR_UMIDADE_PCT` | `40` | Abaixo disso → alerta |
| `SENSOR_SECO` / `SENSOR_UMIDO` | `900` / `300` | Calibração |

Depois de editar: compile e grave de novo.
