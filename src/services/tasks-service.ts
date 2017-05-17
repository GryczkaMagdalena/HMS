import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class TasksService {

  constructor(private httpFetch: HttpFetch) { }

  createTask(taskObj) {
    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Case/Task', {
        method: 'post',
        body: json(taskObj)
      })
        .then(response => console.log(response));
    })
  }
}
