# Lista de materiais

## Hardware obrigatório

| Item | Quantidade | Observação |
|------|------------|------------|
| Arduino Uno ou Nano | 1 | ATmega328P |
| Sensor de umidade do solo | 1 | Resistivo (FC-28 / YL-69) ou capacitivo |
| LED vermelho 5 mm | 1 | Indicador de alerta |
| Resistor 220 Ω (ou 330 Ω) | 1 | Em série com o LED |
| Protoboard | 1 | Para montagem provisória |
| Jumpers macho-macho | 6–10 | Conexões |
| Cabo USB | 1 | Alimentação e gravação |

## Opcional (recomendado)

| Item | Para quê |
|------|----------|
| Resistor 10 kΩ | Pull-down extra se o cabo do sensor for longo |
| Fonte 9 V / 12 V | Alimentar o Arduino sem depender só do USB |
| Caixa / suporte | Proteger a montagem no campo |

## Sensor: resistivo vs capacitivo

| Tipo | Prós | Contras |
|------|------|---------|
| Resistivo (FC-28) | Barato, fácil de achar | Corrói com o tempo no solo |
| Capacitivo | Mais durável | Um pouco mais caro |

Ambos funcionam com este firmware. Ajuste a **calibração** em [`06-calibracao.md`](06-calibracao.md).

## Software

Já previsto neste repositório / ambiente:

- **arduino-cli** (instalado em `~/.local/bin`)
- Core **arduino:avr** (Uno / Nano)

Alternativa gráfica: [Arduino IDE](https://www.arduino.cc/en/software).
