# mini_cpp

Compilador para um subconjunto simplificado de C++ (a gramática está em `arquivos_fornecidos/`), feito para a disciplina de Compiladores. Gera código objeto pra uma máquina virtual hipotética baseada em pilha, e também executa esse código objeto.

O trabalho é dividido em duas partes, como pedido no enunciado: gerar o código objeto a partir do código-fonte, e executar o código objeto gerado.

## Como rodar

Baixe o executável da plataforma correspondente na aba [Releases](../../releases) do repositório (não precisa ter o .NET instalado, o executável já vem com tudo que precisa pra rodar).

No Linux, antes de rodar pela primeira vez, dê permissão de execução:
```
chmod +x Compilador
```

Depois, os comandos são os mesmos nas duas plataformas (no Windows, use `Compilador.exe`; no Linux, `./Compilador`):

```
Compilador -g <caminho_do_codigo_fonte>
```

Compila o arquivo `.cpp` e gera um arquivo `.out` com o mesmo nome, na pasta atual. Por exemplo, `programa.cpp` vira `programa.out`.

```
Compilador -e <caminho_do_codigo_objeto>
```

Lê o `.out` gerado e executa na máquina virtual.

### Rodando a partir do código-fonte (alternativa)

Se preferir compilar você mesmo em vez de usar o executável pronto, com o SDK do .NET instalado, dentro de `src/Compilador`:

```
dotnet run -- -g <caminho_do_codigo_fonte>
dotnet run -- -e <caminho_do_codigo_objeto>
```

## Estrutura do projeto

```
src/Compilador/
├── Traducao/
│   ├── Lexer/
│   ├── Parser/
│   ├── Semantico/
│   └── Geracao/
├── MaquinaVirtual/
└── Program.cs
```

`Traducao/` tem os módulos que traduzem o código-fonte pra código objeto. `MaquinaVirtual/` é quem executa o código objeto depois — não faz parte da tradução em si, só interpreta as instruções.

### Lexer

Lê o código-fonte caractere por caractere e produz uma lista de tokens (`Token.cs`/`TipoToken`). Não faz nenhuma checagem de sintaxe, só reconhece palavras reservadas, identificadores, números, operadores e símbolos.

### Parser

Analisador sintático preditivo (top-down, não recursivo), usando pilha explícita e uma tabela de análise LL(1) (`TabelaM.cs`), gerada a partir da gramática fornecida. A gramática já vinha preparada pra LL(1) (sem recursão à esquerda, sem ambiguidade), então não precisou de nenhuma transformação antes de gerar a tabela.

Além de reconhecer a sintaxe, o Parser também dispara as verificações semânticas e a geração de código na mesma passada, usando um mecanismo de "ações" (`Acao`) inseridas em pontos específicos das produções da tabela M. Isso é explicado com mais detalhe abaixo.

### Semantico

Só faz uma checagem: se uma variável foi declarada antes de ser usada. Não tem checagem de tipos (a linguagem só tem `double`) nem de inicialização.

### Geracao

Responsável por produzir o código objeto:
- `GeradorDeCodigo`: guarda a lista de instruções, emite instruções novas, e corrige saltos condicionais/incondicionais depois (backpatching, usado em `if`/`while`, já que o endereço de destino só é conhecido depois do corpo ser gerado).
- `TabelaDeEnderecos`: mapeia nome de variável pra endereço de memória na VM.

### Como o Parser gera código e faz a análise semântica sem construir uma árvore sintática

O Parser é tabular (pilha + tabela M), não recursivo, e não constrói uma árvore de sintaxe. Pra ainda assim conseguir gerar código e fazer a checagem semântica numa passada só, algumas produções da tabela M têm, além dos símbolos normais (terminais e não-terminais), um terceiro tipo de elemento: uma `Acao`.

Uma `Acao` é só uma função que roda quando ela sai da pilha, no meio do reconhecimento sintático (ex: "quando chegar aqui, verifica se essa variável existe" ou "emite a instrução `SOMA`"). Isso permite emitir código exatamente no ponto certo do processamento, sem esperar o reconhecimento terminar.

Isso funciona bem pra maioria dos casos, mas alguns exigem cuidado. Por exemplo, o operador que vai ser emitido numa soma (`SOMA` ou `SUBT`) é conhecido antes dos dois operandos serem processados, mas só pode ser emitido depois. Esse tipo de informação pendente às vezes precisa ficar numa pilha (não uma variável comum), porque parênteses permitem uma expressão aninhar dentro de outra, e uma variável comum seria sobrescrita pela expressão de dentro antes do valor de fora ser usado. Isso está comentado no código do `Parser.cs`, junto dos campos usados pra guardar esses valores pendentes.

### MaquinaVirtual

Executa o código objeto (lista de instruções). Baseada em pilha: cada instrução ou empilha um valor, desempilha e opera, ou desvia a execução pra outro ponto do programa.

## Sobre uso de IA

Esse README foi escrito com auxílio de IA (Claude, da Anthropic). IA também foi usada ao longo do projeto pra discutir decisões de design e estruturação do compilador (organização de módulos, arquitetura do Parser e da geração de código, etc.).