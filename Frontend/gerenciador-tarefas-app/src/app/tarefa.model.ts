export class Tarefa {
    id: number = 0;
    titulo: string | undefined;
    descricao: string | undefined;
    dataConclusaoPrevista: Date | undefined;
    dataConclusaoEfetiva: Date | undefined;
    situacao: TarefaSituacaoEnum | undefined;

    get concluida(): boolean{
      return this.situacao == TarefaSituacaoEnum.Concluida;
    }

    set concluida(value: boolean){
      this.situacao = ((value || false) ? TarefaSituacaoEnum.Concluida : TarefaSituacaoEnum.Pendente);
    }
}

export enum TarefaSituacaoEnum {
    Pendente = 0,
    Concluida = 1,
}

export enum TiposOrdenacaoTarefas
{
    Situacao = 0,
    Titulo = 1,
    DataConclusaoPrevista = 2,
    DataConclusaoEfetiva = 3,
}