/*
 * Sistema de Irrigação com Arduino
 * --------------------------------
 * Lê o sensor de umidade do solo a cada 100 ms.
 * Se a umidade ficar abaixo do limiar configurado,
 * acende o LED vermelho para o operador abrir o registro.
 *
 * Placa alvo: Arduino Uno / Nano (ATmega328P)
 */

// ====== CONFIGURAÇÃO (ajuste conforme o hardware) ======

const int PIN_SENSOR = A0;          // Entrada analógica do sensor de umidade
const int PIN_LED_VERMELHO = 8;     // Saída digital do LED vermelho (com resistor)

const unsigned long INTERVALO_LEITURA_MS = 100;  // Taxa mínima: 100 ms

// Umidade em %: abaixo deste valor = solo seco → LED acende
const int LIMIAR_UMIDADE_PCT = 40;

// Calibração do sensor (valores brutos do ADC 0–1023)
// Ajuste com o procedimento em docs/04-calibracao.md
const int SENSOR_SECO = 900;   // Leitura típica no ar / solo bem seco
const int SENSOR_UMIDO = 300;  // Leitura típica em solo bem molhado

// Histerese: evita piscar o LED perto do limiar
const int HISTERESE_PCT = 3;

// ====== ESTADO INTERNO ======

unsigned long ultimaLeituraMs = 0;
bool alertaAtivo = false;

void setup() {
  pinMode(PIN_LED_VERMELHO, OUTPUT);
  digitalWrite(PIN_LED_VERMELHO, LOW);

  Serial.begin(9600);
  while (!Serial && millis() < 2000) {
    // Aguarda Serial em placas USB nativas; no Uno segue adiante
  }

  Serial.println(F("=== Sistema de Irrigacao ==="));
  Serial.print(F("Intervalo: "));
  Serial.print(INTERVALO_LEITURA_MS);
  Serial.println(F(" ms"));
  Serial.print(F("Limiar: "));
  Serial.print(LIMIAR_UMIDADE_PCT);
  Serial.println(F("%"));
  Serial.println(F("Bruto\tUmidade%\tAlerta"));
}

void loop() {
  unsigned long agora = millis();

  if (agora - ultimaLeituraMs < INTERVALO_LEITURA_MS) {
    return;
  }
  ultimaLeituraMs = agora;

  int valorBruto = analogRead(PIN_SENSOR);
  int umidadePct = converterParaPercentual(valorBruto);

  atualizarAlerta(umidadePct);
  digitalWrite(PIN_LED_VERMELHO, alertaAtivo ? HIGH : LOW);

  Serial.print(valorBruto);
  Serial.print('\t');
  Serial.print(umidadePct);
  Serial.print('\t');
  Serial.println(alertaAtivo ? F("REGAR") : F("OK"));
}

/**
 * Converte a leitura analógica (0–1023) em umidade aproximada (0–100%).
 * Em sensores resistivos comuns (ex.: FC-28), valor alto = solo seco.
 */
int converterParaPercentual(int valorBruto) {
  int seco = SENSOR_SECO;
  int umido = SENSOR_UMIDO;

  if (seco == umido) {
    return 0;
  }

  // Garante ordem correta mesmo se a calibração estiver invertida
  if (seco < umido) {
    int tmp = seco;
    seco = umido;
    umido = tmp;
  }

  long limitado = constrain(valorBruto, umido, seco);
  int pct = (int)map(limitado, seco, umido, 0, 100);
  return constrain(pct, 0, 100);
}

/**
 * Liga o alerta quando a umidade cai abaixo do limiar.
 * Só desliga quando sobe acima de limiar + histerese.
 */
void atualizarAlerta(int umidadePct) {
  if (!alertaAtivo && umidadePct < LIMIAR_UMIDADE_PCT) {
    alertaAtivo = true;
  } else if (alertaAtivo && umidadePct >= (LIMIAR_UMIDADE_PCT + HISTERESE_PCT)) {
    alertaAtivo = false;
  }
}
