import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { RegisterRequest } from "./register-request";

@Injectable({ providedIn: "root" })
export class RegisterService {

    constructor(private readonly http: HttpClient) {}

    register(request: RegisterRequest) {
        return this.http.post<boolean>("/api/users/create", request);
    }

}