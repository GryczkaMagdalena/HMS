import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class TasksService {

  constructor(private httpFetch: HttpFetch) { }

  createTask(taskObj) {
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
        });
        // .catch(err => reject(err.json()));
    })
  }
}


