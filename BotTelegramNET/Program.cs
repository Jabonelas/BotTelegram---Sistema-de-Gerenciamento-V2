using BotTelegramNET.Context;
using BotTelegramNET.Models;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("6183829865:AAEyfY9L3IaNcBTO1d1m72R3AKdYg8UxvQk");

List<DadosProdutos> listaProdutos = new List<DadosProdutos>();

List<string> listaNomesVendedores = new List<string>();

List<DadosVendedor> listaDadosVendedores = new List<DadosVendedor>();

decimal valorHistorico = 0;

#region Esconder

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"---------------------------------------");
Console.WriteLine($"    Bot @{me.Username} Inicializado");
Console.WriteLine($"---------------------------------------\n");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

#endregion Esconder

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message)
    {
        if (update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                InlineKeyboardButton.WithCallbackData(text: "Histórico de Vendas", callbackData: "Historico"),
                InlineKeyboardButton.WithCallbackData(text: "Faturamento - Top 5 Produtos", callbackData: "Faturamento"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Vendas por Vendedor", callbackData: "Vendas"),
                InlineKeyboardButton.WithCallbackData(text: "Cancelar", callbackData: "Cancelar"),
            },
             });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecione uma das Opções:",
                parseMode: ParseMode.Markdown,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nSolicitacao: Mensagem de Texto  -  Usuario: {update.Message.From.FirstName}  -  ID:{update.Message.From.Id}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Texto Digitado Usuario: {update.Message.Text}");
    }
    else if (update.Type == UpdateType.CallbackQuery)
    {
        var query = update.CallbackQuery.Data;

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nSolicitacao: Comando Selecionado  -  Usuario: {update.CallbackQuery.From.FirstName}  -  ID:{update.CallbackQuery.From.Id}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Comando Solicitado Usuario: {update.CallbackQuery.Data}");

        //
        //Historico
        //
        if (query.Equals("Historico"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Histórico de Hoje", callbackData: "HistoricoHoje"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Histórico dos Últimos 7 Dias", callbackData: "HistoricoUltimos7Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Histórico dos Últimos 30 Dias", callbackData: "HistoricoUltimos30Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancelar", callbackData: "Cancelar"),
                },
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecione um Período:",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("HistoricoHoje"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherHoje();

            string retorno = $"*Histórico Total de Vendas Hoje:* {BuscarHistoricoVenda().ToString("C2")}";

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: retorno,

                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {retorno}");

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("HistoricoUltimos7Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos7Dias();

            string retorno = $"*Histórico Total de Vendas dos Últimos 7 dias:* {BuscarHistoricoVenda().ToString("C2")}";

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: retorno,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {retorno}");

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("HistoricoUltimos30Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos30Dias();

            string retorno = $"*Histórico Total de Vendas dos Últimos 30 dias:* {BuscarHistoricoVenda().ToString("C2")}";

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: retorno,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {retorno}");

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        //
        //Faturamento
        //
        else if (query.Equals("Faturamento"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Faturamento de Hoje", callbackData: "FaturamentoHoje"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Faturamento dos Últimos 7 Dias", callbackData: "FaturamentoUltimos7Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Faturamento dos Últimos 30 Dias", callbackData: "FaturamentoUltimos30Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancelar", callbackData: "Cancelar"),
                },
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecione um Período",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("FaturamentoHoje"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherHoje();

            BuscarProdutosMaisVendidos();

            string produtoTop5 = "*Top 5 Produtos*";

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: produtoTop5,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {produtoTop5}");

            foreach (var item in listaProdutos)
            {
                string retorno = $"*Faturamento de Hoje:*  Cod. Produto: {item.codigoProduto}  -  Descrição Produto {item.nomeProduto}  -  Qtd. Total: {item.quantidade.ToString("N0")} Und.  -  Valor Total: {item.valor.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage3 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("FaturamentoUltimos7Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos7Dias();

            BuscarProdutosMaisVendidos();

            string produtoTop5 = "*Top 5 Produtos*";

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: produtoTop5,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {produtoTop5}");

            foreach (var item in listaProdutos)
            {
                string retorno =
                    $"*Faturamento dos Últimos 7 Dias:*  Cod. Produto: {item.codigoProduto}  -  Descrição Produto {item.nomeProduto}  -  Qtd. Total: {item.quantidade.ToString("N0")} Und.  -  Valor Total: {item.valor.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage3 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("FaturamentoUltimos30Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos30Dias();

            BuscarProdutosMaisVendidos();

            string produtoTop5 = "*Top 5 Produtos*";

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: produtoTop5,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.WriteLine($"Retorno Bot: {produtoTop5}");

            foreach (var item in listaProdutos)
            {
                string retorno =
                    $"*Faturamento dos Últimos 30 Dias:*  Cod. Produto: {item.codigoProduto}  -  Descrição Produto {item.nomeProduto}  -  Qtd. Total: {item.quantidade.ToString("N0")} Und.  -  Valor Total: {item.valor.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage3 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        //
        //Vendas
        //
        else if (query.Equals("Vendas"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Vendas de Hoje", callbackData: "VendasHoje"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Vendas dos Últimos 7 Dias", callbackData: "VendasUltimos7Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Vendas dos Últimos 30 Dias", callbackData: "VendasUltimos30Dias"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancelar", callbackData: "Cancelar"),
                },
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecione um Período",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("VendasHoje"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherHoje();

            BuscarVendedores();

            PecorrerListaVendedor();

            foreach (var item in listaDadosVendedores)
            {
                string retorno =
                    $"*Total de Vendas dos Últimos Hoje:*  Vendedor: {item.vendedor}  -  Qtd Total de Produtos Vendidos: {item.quantidadeTotal.ToString("N")} Und.  -  Valor Total de Produtos Vendidos: {item.valorTotal.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("VendasUltimos7Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos7Dias();

            BuscarVendedores();

            PecorrerListaVendedor();

            foreach (var item in listaDadosVendedores)
            {
                string retorno =
                    $"*Total de Vendas dos Últimos 7 Dias:*  Vendedor: {item.vendedor}  -  Qtd Total de Produtos Vendidos: {item.quantidadeTotal.ToString("N")} Und.  -  Valor Total de Produtos Vendidos: {item.valorTotal.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        else if (query.Equals("VendasUltimos30Dias"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string carregando = "Carregando...";

            Message sentMessage1 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: carregando,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {carregando}");

            PreencherUltimos30Dias();

            BuscarVendedores();

            PecorrerListaVendedor();

            foreach (var item in listaDadosVendedores)
            {
                string retorno =
                    $"*Total de Vendas dos Últimos 30 Dias:*  Vendedor: {item.vendedor}  -  Qtd Total de Produtos Vendidos: {item.quantidadeTotal.ToString("N")} Und.  -  Valor Total de Produtos Vendidos: {item.valorTotal.ToString("C2")}";

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: retorno,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Retorno Bot: {retorno}");
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Sim", callbackData: "sim"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Não", callbackData: "Cancelar"),
                },
            });

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Deseja Algo Mais?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        //
        //Cancelar
        //
        else if (query.Equals("Cancelar"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            string retorno = "Agradecemos o Contato!";

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: retorno,
                cancellationToken: cancellationToken);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Retorno Bot: {retorno}");
        }
        //
        //Sim
        //
        else if (query.Equals("sim"))
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Histórico de Vendas", callbackData: "Historico"),
                    InlineKeyboardButton.WithCallbackData(text: "Faturamento - Top 5 Produtos", callbackData: "Faturamento"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Vendas por Vendedor", callbackData: "Vendas"),
                    InlineKeyboardButton.WithCallbackData(text: "Cancelar", callbackData: "Cancelar"),
                },
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecione uma das Opções:",
                parseMode: ParseMode.Markdown,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}

DateTime dataInicial = DateTime.Today;
DateTime dataFinal = DateTime.Today;

void PreencherHoje()
{
    DateTime hoje = DateTime.Today;

    dataInicial = hoje;
    dataFinal = hoje;
}

void PreencherUltimos7Dias()
{
    DateTime hoje = DateTime.Today;

    dataInicial = hoje;
    dataFinal = hoje.AddDays(-7);
}

void PreencherUltimos30Dias()
{
    DateTime hoje = DateTime.Today;

    dataInicial = hoje;
    dataFinal = hoje.AddDays(-30);
}

void BuscarProdutosMaisVendidos()
{
    try
    {
        using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
        {
            var result = db.tb_nota_fiscal_saida
                .Join(db.tb_estoque, nfs => nfs.fk_estoque, est => est.id_estoque, (nfs, est) => new { nfs, est })
                .Join(db.tb_produto, x => x.est.fk_produto, prod => prod.id_produto, (x, prod) => new { x.nfs, prod })
                .Where(x => x.nfs.nfs_data_emissao <= dataInicial && x.nfs.nfs_data_emissao >= dataFinal)
                .GroupBy(x => new { x.prod.pd_codigo, x.prod.pd_nome })
                .Select(g => new
                {
                    codigoProduto = g.Key.pd_codigo,
                    nomeProduto = g.Key.pd_nome,
                    valor = g.Sum(x => x.nfs.nfs_valor_pago),
                    quantidade = g.Sum(x => x.nfs.nfs_quantidade)
                })
                .OrderByDescending(x => x.quantidade)
                .Take(5)
                .ToList();

            listaProdutos.Clear();

            foreach (var item in result)
            {
                listaProdutos.Add(new DadosProdutos
                {
                    codigoProduto = item.codigoProduto,
                    nomeProduto = item.nomeProduto,
                    quantidade = item.quantidade,
                    valor = item.valor
                });
            }
        }
    }
    catch (Exception x)
    {
        Console.WriteLine(x);
    }
}

decimal BuscarHistoricoVenda()
{
    try
    {
        using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
        {
            decimal valorTotal = db.tb_nota_fiscal_saida.Where(x => x.nfs_data_emissao <= dataInicial && x.nfs_data_emissao >= dataFinal).Sum(x => x.nfs_valor_pago);

            return valorTotal;
        }
    }
    catch (Exception x)
    {
        Console.WriteLine(x);

        return 0;
    }
}

void BuscarFaturamentoPorVendedor(string _vendedor)
{
    try
    {
        using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
        {
            var valoresVendedores = db.tb_nota_fiscal_saida
                .Where(x => x.nfs_vendedor == _vendedor && x.nfs_data_emissao <= dataInicial && x.nfs_data_emissao >= dataFinal)
                .GroupBy(x => x.nfs_vendedor)
                .OrderBy(x => x.Key)
                .Select(g => new
                {
                    TotalQuantidade = g.Sum(x => x.nfs_quantidade),
                    TotalValorPago = g.Sum(x => x.nfs_valor_pago)
                }).ToList();

            foreach (var item in valoresVendedores)
            {
                listaDadosVendedores.Add(new DadosVendedor { vendedor = _vendedor, quantidadeTotal = item.TotalQuantidade, valorTotal = item.TotalValorPago });
            }
        }
    }
    catch (Exception x)
    {
        Console.WriteLine(x);
    }
}

void BuscarVendedores()
{
    try
    {
        using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
        {
            var vendedores = db.tb_registro.Where(x => x.rg_tipo_cadastro.Equals("Funcionario")).Select(x => new { x.rg_login, x.rg_tipo_cadastro }).ToList();

            listaNomesVendedores.Clear();

            foreach (var item in vendedores)
            {
                listaNomesVendedores.Add(item.rg_login);
            }
        }
    }
    catch (Exception x)
    {
        Console.WriteLine(x);
    }
}

void PecorrerListaVendedor()
{
    foreach (var item in listaNomesVendedores)
    {
        BuscarFaturamentoPorVendedor(item);
    }
}

internal struct DadosVendedor
{
    public string vendedor { get; set; }
    public decimal quantidadeTotal { get; set; }
    public decimal valorTotal { get; set; }
}

public struct DadosProdutos
{
    public string codigoProduto { get; set; }
    public string nomeProduto { get; set; }
    public decimal quantidade { get; set; }
    public decimal valor { get; set; }
}