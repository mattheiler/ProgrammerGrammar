export class LoginError {
    constructor(error?: Partial<LoginError>) {
        Object.assign(this, error || {});
    }

    messages: string[] = [];
}