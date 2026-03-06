namespace ContratoApp.Dominio.Events;

public sealed record ClienteCriadoEvent(Guid IdCliente) : IEvent;
public sealed record ClienteAtualizadoEvent(Guid IdCliente) : IEvent;

public sealed record ContratoCriadoEvent(Guid IdContrato, Guid IdCliente) : IEvent;
public sealed record ContratoAtualizadoEvent(Guid IdContrato, Guid IdCliente) : IEvent;

public sealed record OrdemServicoCriadaEvent(Guid IdOrdemServico, Guid IdCliente) : IEvent;
public sealed record OrdemServicoAtualizadaEvent(Guid IdOrdemServico, Guid IdCliente) : IEvent;

public sealed record OrdemServicoItemAdicionadoEvent(Guid IdOrdemServico, string Descricao) : IEvent;
public sealed record OrdemServicoItemAtualizadoEvent(Guid IdOrdemServico, string Descricao) : IEvent;
public sealed record OrdemServicoItemRemovidoEvent(Guid IdOrdemServico, string Descricao) : IEvent;
