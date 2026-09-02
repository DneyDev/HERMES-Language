# Linguagem HERMES

Linguagem de programação educacional baseada em Python, adaptada para o português e desenvolvida em C#.

**Criador:** Sidney Rodrigues — ex-Desenvolvedor Backend do Projeto H.E.R.M.E.S.

---

## Objetivo

A Linguagem HERMES foi criada para ser utilizada em um jogo educacional desenvolvido em Unity pela equipe do Núcleo de Tecnologia Educacional (NTE) da Secretaria de Educação e Esportes de Paulista — o **Projeto H.E.R.M.E.S.**

Seu objetivo é permitir que jogadores de 9 a 13 anos aprendam conceitos de programação por meio de puzzles e desafios dentro do jogo, com uma sintaxe simples, controlada e em português, inspirada em Python — sem a necessidade de implementar todos os recursos da linguagem original.

## Características

- Baseada em Python
- Sintaxe em português
- Desenvolvida em C#
- Foco educacional
- Público-alvo de 9 a 13 anos
- Sintaxe simplificada
- Tipagem implícita
- Integração planejada com Unity
- Conjunto controlado de recursos da linguagem

## 🛠️ Desenvolvimento

A Linguagem HERMES está sendo desenvolvida como um interpretador próprio em C#.

O fluxo de execução atualmente é:

```
Código HERMES
      ↓
    Lexer
      ↓
    Tokens
      ↓
    Parser
      ↓
     AST
      ↓
 Interpreter
      ↓
   Runtime
      ↓
    Unity
```

### Lexer

Responsável por transformar o código HERMES em uma sequência de tokens.

Suporte atual:

- Números
- Strings
- Booleanos
- Identificadores
- Palavras-chave
- Operadores matemáticos
- Operadores de comparação
- Operadores lógicos
- Atribuição
- Parênteses
- Dois-pontos
- Vírgulas
- Quebras de linha
- Indentação / Dedentação
- Comentários

Palavras-chave implementadas: `se`, `senao`, `e`, `ou`, `nao`, `enquanto`, `Verdadeiro`, `Falso`.

> As palavras-chave da linguagem não utilizam acentos.

### Parser

Transforma os tokens produzidos pelo Lexer em uma AST (Abstract Syntax Tree).

Suporte atual:

- Atribuição de variáveis
- Expressões matemáticas
- Comparações
- Operadores lógicos
- Operador `nao`
- Parênteses
- Estruturas condicionais (`se` / `senao`)
- Laços `enquanto`
- Chamadas de funções e argumentos

**Precedência de expressões** (da mais alta para a mais baixa):

```
nao
 ↓
* /
 ↓
+ -
 ↓
comparações
 ↓
e
 ↓
ou
```

Parênteses possuem prioridade sobre as demais operações.

### Interpreter

Responsável por executar a AST produzida pelo Parser.

Suporte atual:

- Declaração e leitura de variáveis
- Operações matemáticas
- Comparações
- Operadores lógicos
- Estruturas condicionais
- Laços `enquanto`
- Chamadas de funções
- Validação de tipos
- Controle de execução

> O laço `enquanto` possui um limite máximo de **10.000 execuções** para evitar loops infinitos.

### Runtime

Contém os recursos necessários para a execução da linguagem:

- Ambiente de variáveis
- Registro de funções
- Sistema de funções externas
- Sistema de erros da linguagem

As funções disponíveis para o código HERMES são registradas por meio de uma interface própria, permitindo que recursos externos sejam disponibilizados para a linguagem — a ideia é usar esse sistema futuramente para conectar comandos da linguagem às funcionalidades do jogo em Unity.

Exemplos atuais de funções registradas:

```
mostrar()
abrir_porta()
```

## Tipos de Dados

A linguagem possui inicialmente três tipos principais: **Número**, **Texto** e **Booleano**.

```python
idade = 10
nome = "Lara"
ativo = Verdadeiro
```

As variáveis utilizam tipagem implícita e **não há conversão automática entre tipos diferentes** — operações matemáticas exigem números, e a concatenação com `+` aceita apenas strings com strings.

## ⚙️ Operadores

| Categoria    | Operadores                      |
|--------------|----------------------------------|
| Matemáticos  | `+` `-` `*` `/`                  |
| Comparação   | `==` `!=` `>` `<` `>=` `<=`      |
| Lógicos      | `e` `ou` `nao`                   |

## Sistema de Erros

A linguagem possui uma hierarquia própria de exceções baseada em `HermesException`.

Erros específicos já implementados:

- Variável não definida
- Função não registrada
- Tipo inválido
- Divisão por zero
- Quantidade incorreta de argumentos em funções

Exemplo:

```
=== ERRO HERMES ===
Mensagem: Variável 'idade' não foi definida.
```

O objetivo desse sistema é fornecer mensagens de erro mais claras e específicas para quem estiver utilizando a linguagem.

## Estrutura do Projeto

```
HERMES-Lang/
├── Lexer/
│   ├── Lexer.cs
│   ├── Token.cs
│   └── TokenType.cs
│
├── Parser/
│   ├── ASTPrinter.cs
│   ├── AssignmentStatement.cs
│   ├── BinaryExpression.cs
│   ├── CallExpression.cs
│   ├── Expression.cs
│   ├── ExpressionStatement.cs
│   ├── IfStatement.cs
│   ├── LiteralExpression.cs
│   ├── Parser.cs
│   ├── ParserException.cs
│   ├── Statement.cs
│   ├── UnaryExpression.cs
│   └── WhileStatement.cs
│
├── Interpreter/
│   └── Interpreter.cs
│
├── Runtime/
│   ├── Environment.cs
│   ├── FunctionRegistry.cs
│   ├── HermesException.cs
│   ├── IHermesFunction.cs
│   │
│   └── Errors/
│       ├── UndefinedVariableException.cs
│       ├── UndefinedFunctionException.cs
│       ├── InvalidTypeException.cs
│       ├── DivisionByZeroException.cs
│       └── FunctionArityException.cs
│
├── Runtime/Functions/
│   ├── MostrarFunction.cs
│   └── AbrirPortaFunction.cs
│
├── Program.cs
├── HERMESLANG.csproj
└── .gitignore
```

## Exemplo de Código

```python
contador = 0

enquanto contador < 5:
    contador = contador + 1
```

Estruturas condicionais:

```python
vida = 100

se vida > 50:
    mostrar("Vida alta")
senao:
    mostrar("Vida baixa")
```

## Integração com o Projeto H.E.R.M.E.S.

A Linguagem HERMES foi projetada para funcionar como uma camada de programação dentro do jogo. O jogador poderá escrever comandos em português para resolver desafios e interagir com elementos do ambiente.

A arquitetura de funções permite que o interpretador permaneça separado da lógica específica do jogo, possibilitando futuramente registrar funções responsáveis por executar ações dentro da Unity.

Exemplo conceitual:

```python
abrir_porta()
mostrar("Porta aberta!")
```

## Estado Atual

A linguagem encontra-se em desenvolvimento. Os principais componentes da primeira versão do interpretador já foram implementados:

- Lexer, Tokens, Indentação e dedentação
- Parser e AST
- Variáveis, operações matemáticas, comparações, operadores lógicos, parênteses, strings
- Validação de tipos
- Estruturas `se` / `senao`
- Estrutura `enquanto`
- Sistema de funções registradas
- Runtime
- Sistema de erros

Ainda existem etapas futuras para expansão da linguagem e integração completa com a Unity.

## Objetivo Futuro

A evolução da Linguagem HERMES deverá priorizar:

- Expansão controlada dos recursos da linguagem
- Melhorias no sistema de erros (incluindo informações de linha nos erros)
- Testes automatizados
- Novos comandos educacionais
- Integração com funcionalidades da Unity
- Uso da linguagem nos puzzles do Projeto H.E.R.M.E.S.

---
