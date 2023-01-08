class Base:
    server_url: str = ""
    server_connect_api = None

    def set_user_base_api(self, base_api: str, user_name: str):
        if user_name == "":
            self.server_url = base_api
        self.server_url = f"{base_api}/{user_name}"


