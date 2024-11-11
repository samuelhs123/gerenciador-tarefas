import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TarefaService } from '../tarefa.service';
import { Tarefa, TarefaSituacaoEnum, TiposOrdenacaoTarefas } from '../tarefa.model';

@Component({
  selector: 'app-tarefas-list',
  templateUrl: './tarefas-list.component.html',
  styleUrls: ['./tarefas-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class TarefasListComponent implements OnInit {
  tarefas: Tarefa[] = [];
  displayedColumns: string[] = ['titulo', 'descricao', 'dataConclusaoPrevista', 'situacao', 'dataConclusaoEfetiva', 'opcoes'];
  filtroSituacaoSelecionado = [TarefaSituacaoEnum.Pendente, TarefaSituacaoEnum.Concluida];
  ordenacaoSelecionada: TiposOrdenacaoTarefas = TiposOrdenacaoTarefas.Situacao;
  filtroSituacaoSelecionadoBind: string[] = [];
  ordenacaoSelecionadaBind: string = '';

  constructor(private tarefaService: TarefaService) { }

  ngOnInit() {
    this.filtroSituacaoSelecionadoBind = this.filtroSituacaoSelecionado.map(v => `${v}`);
    this.ordenacaoSelecionadaBind = `${this.ordenacaoSelecionada}`;
    this.filtroSituacaoSelecionado = this.filtroSituacaoSelecionadoBind.map(v => Number(v) as TarefaSituacaoEnum);
    this.ordenacaoSelecionada = Number(this.ordenacaoSelecionadaBind[0]) as TiposOrdenacaoTarefas;
    this.atualizarLista();
  }

  atualizarLista(){
    this.tarefaService.getTarefas(this.filtroSituacaoSelecionado, this.ordenacaoSelecionada).subscribe(tarefas => this.tarefas = tarefas);
  }

  updateSituacaoTarefa(tarefa: Tarefa) {
    tarefa.concluida = !tarefa.concluida;

    this.tarefaService.updateTarefa(tarefa).subscribe(resp => {
      if (resp.sucesso){
        const tarefaAlterada = resp.retorno as Tarefa;
        const tarefaLista = this.tarefas.find(t => t.id == tarefa.id);
        if (tarefaLista){
          tarefaLista.dataConclusaoEfetiva = tarefaAlterada.dataConclusaoEfetiva;
          tarefaLista.concluida = tarefaAlterada.concluida;
        }
      }
    });
  }

  deleteTarefa(id: number) {
    this.tarefaService.deleteTarefa(id).subscribe(() => {
      this.tarefas = this.tarefas.filter(tarefa => tarefa.id !== id);
    });
  }

  filtroSituacaoChange(valorFiltro: string[]){
    this.filtroSituacaoSelecionado = valorFiltro.map(v => Number(v) as TarefaSituacaoEnum);
    this.atualizarLista();
  }

  ordenacaoChange(opcaoOrdenacao: string){
    this.ordenacaoSelecionada = Number(opcaoOrdenacao) as TiposOrdenacaoTarefas;
    this.atualizarLista();
  }
}