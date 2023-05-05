# Projeto “C#: Paralelismo no mundo real”.

Curso introdutório a threads, tasks e schedulers.

| :placard: Vitrine.Dev |                                             |
| --------------------- | ------------------------------------------- |
| :sparkles: Nome       | **ByteBank processamento de contas**        |
| :label: Tecnologias   | c#, .Net 7, Blazor (tecnologias utilizadas) |

<!-- Inserir imagem com a #vitrinedev ao final do link -->

![Exemplo de execução do processamento](https://i.imgur.com/aLqEUtA.gif#vitrinedev)

## Detalhes do projeto

O código, que foi disponibilizado pelo instrutor, trata de um app em .Net Framework 4 e Xamarin que deve processar as movimentações em conta e apresentar o saldo atualizado de um grupo de clientes. O problema a ser resolvido é que esse processamento pesado torna o app muito lento.<br><br>
Como o Xamarin não tem portabilidade para linux, precisei adaptar o projeto e após fazer uma pesquisa, me deparei com uma outra alternativa, criar o projeto usando Blazor e depois usar o Electron para torná-lo uma aplicação desktop multiplataforma. Decidi seguir por essa linha utilizando a última versão do dotnet, o .Net 7, e criei o projeto como uma aplicação Blazor server. No geral, cada componente blazor contém um trecho de html, com bootstrap ou css puro para estilização, seguido do trecho de código c# onde programamos as funcionalidade da página, e é isso, sem javascript no meio.
Fui atualizando o layout usando os recursos do bootstrap para ficar o mais próximo do apresentado nas aulas.<br><br>
Como a maioria das coisas demonstradas no curso foram em relação ao funcionamento do paralelismo em c#, não foi difícil adaptar o projeto, porém, infelizmente não consegui usar o electron para tornar o projeto em uma aplicação desktop porque ainda não atualizaram o framework para funcionar com as versões mais novas do dotnet, então segue sendo um browser app.
