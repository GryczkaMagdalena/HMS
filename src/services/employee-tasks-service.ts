import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';
import {LoadHandlerService} from "./load-handler-service";


@inject(HttpFetch, LoadHandlerService)
export class EmployeeTasksService {

  constructor(private httpFetch: HttpFetch, private loadHandlerService: LoadHandlerService) {
  }

  getTasks() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Task')
        .then(response => response.json())
        .then(data => {
          resolve(data);
        })
        .catch(err => reject(err))

        .then(() => {
          this.loadHandlerService.setFree();
        });
    });
  }

  getCurrentShift() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Worker/CurrentShift')
        .then(response => response.json())
        .then(data => resolve(data))
        .catch(err => reject(err))
        .then(() => {
          this.loadHandlerService.setFree();
        });
    })


  }
}
