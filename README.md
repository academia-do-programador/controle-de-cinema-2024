# Controle de Cinemas

Com a necessidade de gerenciar melhor as vendas de ingressos do Cinema de Lages, foi proposto pelo dono do Cinema o
Sr. João do Nascimento, a criação de um sistema que auxilia no controle das vendas dos ingressos para os clientes que
desejam assistir os filmes e comer aquela pipoca nos finais de semana.
A equipe do Cinema deseja otimizar o controle de um conjunto de salas e agilizar o processo de venda dos ingressos aos
clientes. Atualmente, a equipe utiliza formulários de papel para armazenar as informações dos ingressos que já foram
vendidos e ainda os ingressos que estão disponíveis, como também as capacidades das salas.

O Cinema, possui muitas salas sendo necessário, portanto, registrar informações a respeito de cada uma, como sua
capacidade, o número de assentos disponíveis etc. O cinema apresenta também muitos filmes e um filme tem suas
informações mais importantes como título e duração. Um filme tem também o gênero.

Ao chegar um lançamento, os funcionários devem ter a possibilidade registrar no acervo do cinema.
Um mesmo filme pode ser apresentado em salas e horários diferentes, constituindo-se uma sessão. Uma sessão tem um
número máximo de ingressos colocados à venda, determinado pela capacidade da sala onde a sessão acontece;

A venda de ingressos é intermediada por um funcionário do cinema. Um ingresso deve conter informações como o tipo de
ingresso (inteiro ou meio ingresso) e, além disso, um cliente só pode comprar ingressos para sessões ainda não
encerradas.

Considere a sessão e a venda de ingressos como os elementos centrais desta aplicação.
O funcionário do cinema deve ser capaz de visualizar as sessões do dia agrupados por filme, tanto as em andamento
quanto aquelas ainda por serem iniciadas.

Atendendo à solicitação de um cliente, o funcionário deverá efetuar a venda de um ou mais ingressos, obedecendo à
capacidade máxima de cada sala.

O Sistema de Controle de Cinema deve possuir um módulo de cadastro, onde serão mantidas, no mínimo, as sessões. Este
módulo deve permitir a consulta, inclusão, alteração e remoção de sessões.

O sistema deve apresentar os detalhes das sessões, mostrando as poltronas disponíveis e já vendidas.

## Requisitos

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.

---

## Como Usar

#### Clone o Repositório
```
git clone https://github.com/academia-do-programador/controle-de-cinema-2024.git
```

#### Navegue até a pasta raiz da solução
```
cd controle-de-cinema-2024
```

#### Restaure as dependências
```
dotnet restore
```

#### Navegue até a pasta do projeto
```
cd ControleCinema.WebApp
```

#### Execute o projeto
```
dotnet run
```

#### Abra a página inicial no navegador
```
http://localhost:9001/
```