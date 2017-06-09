import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class EmployeeTasksService {

	constructor(private httpFetch: HttpFetch) {
	}

	getTasks(){
		let promise = new Promise((resolve, reject) => {
			this.httpFetch.fetch('/api/Task')
				.then(response => response.json())
				.then(data => {
					console.log(data);
					resolve(data);
				}).catch(err => reject(err));
		});
		return promise;
	}
}