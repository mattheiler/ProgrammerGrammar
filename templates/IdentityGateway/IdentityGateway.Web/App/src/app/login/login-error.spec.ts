import { LoginError } from "./login-error";

describe("LoginError",
    () => {
        it("should create an instance",
            () => {
                expect(new LoginError()).toBeTruthy();
            });
    });