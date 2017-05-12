import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class HotelRulesService {
	data;

	constructor(private httpFetch: HttpFetch) {
	}

	getRules(){
		let promise = new Promise((resolve, reject) => {
			if(!this.data){
				this.httpFetch.fetch('/api/Home')
				.then(response => response.json())
				.then(data => {
					this.data = data;
					resolve(data);
				}).catch(err => reject(err));
			}
			else
				resolve(this.data);
		});
		return promise;
	}

	getRule(ruleID){
		return this.data.find(item => item.ruleID == ruleID);
	}
}