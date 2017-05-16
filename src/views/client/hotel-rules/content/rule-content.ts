import {inject} from 'aurelia-framework';
import {HotelRulesService} from '../../../../services/hotel-rules-service';

@inject(HotelRulesService)
export class RuleContent {
	selectedRule;
	constructor(private hotelRulesService: HotelRulesService) {
	}

	activate(params, routeConfig){
		this.hotelRulesService.getRule(params.id).then(data => this.selectedRule = data);
	}
}