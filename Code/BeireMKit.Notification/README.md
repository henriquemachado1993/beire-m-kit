# BeireMKit Notification
A biblioteca BeireMKit Notification foi desenvolvida para facilitar a manipulação de mensagens do sistema, utilizando o padrão Notification.

## Funcionalidades

1. Manipular mensagens do sistema de forma simplificada.

## Pré-requisitos
Certifique-se de ter instalado o .NET Core 6 SDK em sua máquina antes de começar.

## Como Usar
* Adicionar o serviço de notification no Startup
	* No método ConfigureServices da classe Startup, adicione o serviço de notification usando o método ConfigureNotification(): 
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureNotification();
    }
    ```