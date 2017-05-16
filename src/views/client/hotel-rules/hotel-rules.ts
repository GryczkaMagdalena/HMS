import {inject} from 'aurelia-framework';
import {RouterConfiguration, Router} from 'aurelia-router';

@inject(Router)
export class HotelRules {

	constructor(private router: Router){
	}
	
	configureRouter(config: RouterConfiguration, router: Router) {
		config.map([
		{
			route: '',
			name: 'noSelection',
			moduleId: 'views/client/hotel-rules/no-selection',
		},
		{
			route: 'rule-content/:id',
			name: 'ruleRouter',
			moduleId: 'views/client/hotel-rules/content/rule-content',
		}
		]);
		this.router = router;
	}
}
