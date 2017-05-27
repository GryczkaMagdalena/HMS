import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';
import {LoadHandlerService} from "./load-handler-service";


@inject(HttpFetch, LoadHandlerService)
export class TasksService {

  constructor(private httpFetch: HttpFetch, private loadHandlerService: LoadHandlerService) { }

  createTask(taskObj) {
    this.loadHandlerService.setBusy();

    let task = {
      title: taskObj.title,
      describe: taskObj.description,
      email: sessionStorage.getItem('user_email'),
      roomNumber: sessionStorage.getItem('room_number')
    };
    console.log('task', task);

    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Task', {
        method: 'post',
        body: JSON.stringify(task)
      })
        .then(response => resolve(response))
        .catch(err => {
          err.json().then(status => reject(status));
        })
        .then(() => this.loadHandlerService.setFree());
    })
  }
}


