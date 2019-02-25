export default class RecurrenceTemplate {
    constructor() {
        this.type = 0;
        this.end_type = 0;
        this.end_date = null;
        this.end_count = 1;
        this.repeat_period_type = 0;
        this.interval = 1;
        this.timespan_start = null;
        this.timespan_end = null;
        this.allow_monday = true;
        this.allow_tuesday = true;
        this.allow_wednesday = true;
        this.allow_thursday = true;
        this.allow_friday = true;
        this.allow_saturday = true;
        this.allow_sunday = true;
        this.repeat_month_type = 0;
        this.recurrence_change_type = 0;
    }
}
