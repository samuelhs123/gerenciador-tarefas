import { Injectable } from '@angular/core';
import { Tarefa, TarefaSituacaoEnum, TiposOrdenacaoTarefas } from './tarefa.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { RespostaAPI } from './respostaAPI.model';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {
  private apiUrl = environment.API_SERVER_URL;

  constructor(private http: HttpClient) { }

  getTarefas(valorFiltro: TarefaSituacaoEnum[], ordenacao: TiposOrdenacaoTarefas): Observable<Tarefa[]> {
    let url = `${this.apiUrl}?`;
    valorFiltro.forEach(v => url += (url.endsWith('?') ? '' : '&') + `situacoes=${v}`);
    url += (url.endsWith('?') ? '' : '&') + `ordenacao=${ordenacao}`;

    return this.http.get<RespostaAPI>(url).pipe(
      map(ret => {
        if (!ret.sucesso){
          alert(ret.mensagem);
          return [];
        }

        const tarefas = ret.retorno as Tarefa[];
        for (let i = 0; i < tarefas.length; i++) {
          tarefas[i] = Object.assign(new Tarefa(), tarefas[i]);
       }        

        return ret.retorno as Tarefa[];
      })
    );
  }

  getTarefa(id: number): Observable<Tarefa> {
    return this.http.get<RespostaAPI>(`${this.apiUrl}/${id}`).pipe(
      map(ret => {
        if (!ret.sucesso){
          alert(ret.mensagem);
          return new Tarefa();
        }

        return Object.assign(new Tarefa(), ret.retorno as Tarefa);;
      })
    );
  }

  addTarefa(tarefa: Tarefa): Observable<RespostaAPI> {
    return this.http.post<RespostaAPI>(this.apiUrl, tarefa).pipe(
      map(ret => {
        if (ret.sucesso){
          ret.retorno = Object.assign(new Tarefa(), ret.retorno as Tarefa);
        }
        else{
          alert(ret.mensagem);
        }

        return ret;
      })
    );
  }

  updateTarefa(tarefa: Tarefa): Observable<RespostaAPI> {
    return this.http.put<RespostaAPI>(`${this.apiUrl}/${tarefa.id}`, tarefa).pipe(
      map(ret => {
        if (ret.sucesso){
          ret.retorno = Object.assign(new Tarefa(), ret.retorno as Tarefa);
        }
        else{
          alert(ret.mensagem);
        }

        return ret;
      })
    );
  }

  deleteTarefa(id: number): Observable<void> {
    return this.http.delete<RespostaAPI>(`${this.apiUrl}/${id}`).pipe(
      map(ret => {
        if (!ret.sucesso){
          alert(ret.mensagem);
        }

        return;
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    alert(error.error.mensagem);
    return throwError(() => new Error(error.message));
  }
}