import { Component, OnInit } from '@angular/core';
import { TarefaService } from '../tarefa.service';
import { Tarefa, TarefaSituacaoEnum } from '../tarefa.model';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-tarefa-detail',
  templateUrl: './tarefa-detail.component.html',
  styleUrls: ['./tarefa-detail.component.css']
})
export class TarefaDetailComponent implements OnInit {
  tarefa: Tarefa = new Tarefa();

  constructor(
    private tarefaService: TarefaService,
    private route: ActivatedRoute,
    private router: Router,
    private datePipe: DatePipe,
  ) { }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id') || '0');

    if (id > 0){
      this.tarefaService.getTarefa(id).subscribe(tarefa => this.tarefa = tarefa);
    }
  }

  get dataConclusaoPrevistaFormattedDate() {
    return this.datePipe.transform(this.tarefa.dataConclusaoPrevista, 'yyyy-MM-dd') || '';
  }

  salvar(){
    this.tarefa.id > 0 ? this.updateTarefa() : this.addTarefa();
  }

  addTarefa() {
    this.tarefaService.addTarefa(this.tarefa).subscribe(resp => {
      if (resp.sucesso){
        this.reset();
      }
    });
  }

  updateTarefa() {
    this.tarefaService.updateTarefa(this.tarefa).subscribe(resp => {
      if (resp.sucesso){
        this.reset();
      }
    });
  }

  reset() {
    this.router.navigate(['/tarefas']);
  }

  teste(e: any){
    this.tarefa.situacao = (e == 1 ? TarefaSituacaoEnum.Concluida : TarefaSituacaoEnum.Pendente);
    //this.tarefa.concluida = (e == 1) as boolean;
  }
}
