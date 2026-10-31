/*
 * Simulador do Sistema de Irrigação (sem Arduino físico)
 * Replica a lógica de irrigacao/irrigacao.ino
 *
 * Controles:
 *   ↑ / +     aumentar umidade
 *   ↓ / -     diminuir umidade
 *   A         modo automático (solo seca sozinho)
 *   R         simular rega (umidade sobe rápido)
 *   Q / Esc   sair
 */

using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

const int LimiarUmidadePct = 40;
const int HisteresePct = 3;
const int SensorSeco = 900;
const int SensorUmido = 300;
const int IntervaloLeituraMs = 100;

int umidadePct = 55;
bool alertaAtivo = false;
bool modoAuto = true;
var relogio = Stopwatch.StartNew();
long ultimaLeituraMs = 0;
long ultimoAutoMs = 0;

DesenharAjudaInicial();

while (true)
{
    try
    {
        while (Console.KeyAvailable)
        {
            var tecla = Console.ReadKey(intercept: true).Key;
            switch (tecla)
            {
                case ConsoleKey.OemPlus:
                case ConsoleKey.Add:
                case ConsoleKey.UpArrow:
                    umidadePct = Math.Min(100, umidadePct + 5);
                    modoAuto = false;
                    break;
                case ConsoleKey.OemMinus:
                case ConsoleKey.Subtract:
                case ConsoleKey.DownArrow:
                    umidadePct = Math.Max(0, umidadePct - 5);
                    modoAuto = false;
                    break;
                case ConsoleKey.A:
                    modoAuto = !modoAuto;
                    break;
                case ConsoleKey.R:
                    umidadePct = Math.Min(100, umidadePct + 25);
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    Console.ResetColor();
                    Console.Clear();
                    Console.WriteLine("Simulador encerrado.");
                    return;
            }
        }
    }
    catch (InvalidOperationException)
    {
        // Entrada redirecionada (pipe) — rode no Terminal para usar o teclado
    }

    long agora = relogio.ElapsedMilliseconds;

    // Solo seca ~1% a cada 400 ms no modo automático
    if (modoAuto && agora - ultimoAutoMs >= 400)
    {
        ultimoAutoMs = agora;
        if (umidadePct > 0)
        {
            umidadePct--;
        }
    }

    if (agora - ultimaLeituraMs < IntervaloLeituraMs)
    {
        Thread.Sleep(10);
        continue;
    }

    ultimaLeituraMs = agora;

    // Mesma regra do firmware (.ino)
    if (!alertaAtivo && umidadePct < LimiarUmidadePct)
    {
        alertaAtivo = true;
    }
    else if (alertaAtivo && umidadePct >= LimiarUmidadePct + HisteresePct)
    {
        alertaAtivo = false;
    }

    int valorBruto = PercentualParaBruto(umidadePct, SensorSeco, SensorUmido);
    DesenharPainel(umidadePct, valorBruto, alertaAtivo, modoAuto, LimiarUmidadePct, IntervaloLeituraMs);
}

static int PercentualParaBruto(int pct, int sensorSeco, int sensorUmido)
{
    pct = Math.Clamp(pct, 0, 100);
    // Inverso do map do Arduino: seco=900 → 0%, úmido=300 → 100%
    return sensorSeco - (int)((sensorSeco - sensorUmido) * (pct / 100.0));
}

static void DesenharAjudaInicial()
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════════════╗");
    Console.WriteLine("║   SIMULADOR — Sistema de Irrigação Arduino       ║");
    Console.WriteLine("║   (mesma lógica do firmware .ino, sem hardware)  ║");
    Console.WriteLine("╚══════════════════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine("  Controles:");
    Console.WriteLine("    ↑ / +     aumentar umidade");
    Console.WriteLine("    ↓ / -     diminuir umidade");
    Console.WriteLine("    R         simular REGAR (abre o registro)");
    Console.WriteLine("    A         ligar/desligar modo automático");
    Console.WriteLine("    Q / Esc   sair");
    Console.WriteLine();
    Console.WriteLine("  Iniciando painel...");
    Thread.Sleep(900);
    Console.Clear();
}

static void DesenharPainel(
    int umidadePct,
    int valorBruto,
    bool alertaAtivo,
    bool modoAuto,
    int limiar,
    int intervaloMs)
{
    Console.SetCursorPosition(0, 0);
    Console.WriteLine("╔══════════════════════════════════════════════════╗");
    Console.WriteLine("║   SIMULADOR — Sistema de Irrigação Arduino       ║");
    Console.WriteLine("╚══════════════════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine($"  Limiar: {limiar}%   |   Leitura: a cada {intervaloMs} ms          ");
    Console.WriteLine($"  Modo:   {(modoAuto ? "AUTOMÁTICO (solo secando) " : "MANUAL                   ")}");
    Console.WriteLine();

    Console.Write("  Umidade do solo: ");
    Console.ForegroundColor = umidadePct < limiar ? ConsoleColor.Yellow : ConsoleColor.Cyan;
    Console.Write($"{umidadePct,3}%");
    Console.ResetColor();
    Console.WriteLine($"   (ADC bruto simulado: {valorBruto})     ");
    Console.WriteLine();
    Console.WriteLine($"  [{Barra(umidadePct)}] {umidadePct}%   ");
    Console.WriteLine();

    Console.Write("  LED vermelho: ");
    if (alertaAtivo)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("● ACESO  →  operador deve ABRIR O REGISTRO     ");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("○ apagado → umidade OK                         ");
    }
    Console.ResetColor();
    Console.WriteLine();

    Console.Write("  Alerta serial: ");
    Console.ForegroundColor = alertaAtivo ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine(alertaAtivo ? "REGAR    " : "OK       ");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine("  Controles: ↑/↓ umidade | R regar | A auto | Q sair     ");
}

static string Barra(int pct)
{
    int cheios = pct / 5;
    return new string('█', cheios) + new string('░', 20 - cheios);
}
