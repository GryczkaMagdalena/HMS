import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class CasesService {
  cases: {}[];

  constructor(private httpFetch: HttpFetch) {
    this.cases = [
      {
        CaseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273801",
        Title:"Wymiana żarówki",
        Description:'Wymiana żarówki',
        WorkerType:"Technician"
      },
      {
        CaseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273802",
        Title:"Naprawa łóżka",
        Description:'Naprawa łóżka',
        WorkerType:"Technician"
      },
      {
        CaseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273803",
        Title:"Naprawa telewizora",
        Description:'Naprawa telewizora',
        WorkerType:"Technician"
      },
      {
        CaseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273804",
        Title:"Inne",
        Description:'Inne',
        WorkerType:"Technician"
      },
      {
        CaseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273805",
        Title:"ExampleCase",
        Description:"Clean something",
        WorkerType:"Technician"
      }
    ]
  }

  getTechnicianCases() {
    // return new Promise((resolve, reject) => {
    //   this.httpFetch.fetch('/api/Case')
    //     .then(response => response.json())
    //     .then(data => console.log(data))
    //     .catch(err => reject(err));
    // });
    return new Promise((resolve, reject) => {
      resolve(this.cases);
    });
  }

  getCleanerCases() {
    // return new Promise((resolve, reject) => {
    //   this.httpFetch.fetch('/api/Case')
    //     .then(response => response.json())
    //     .then(data => console.log(data))
    //     .catch(err => reject(err));
    // });
    return new Promise((resolve, reject) => {
      resolve(this.cases);
    });
  }
}
