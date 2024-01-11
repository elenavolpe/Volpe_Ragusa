from utils import connect_go_server, is_valid_email, is_valid_password, is_valid_username

# (account) deve essere dizionario con i campi 'email' e 'password'
def login(account):
    if type(account) is not dict:
        raise TypeError(f"Errore login: account deve essere un dizionario, invece è di tipo {type(account)}")
    
    if is_valid_email(account['email']) and is_valid_password(account['password']):
        try:
            r = connect_go_server('login', account)
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "Errore formato email e/o password non validi"

# In questo caso account deve essere un dizionario con i campi 'email', 'password', 'name', 'surname' e 'username'
def signin(account):
    if type(account) is not dict:
        raise TypeError(f"Errore signin: account deve essere un dizionario, invece è di tipo {type(account)}")
    
    if is_valid_email(account['email']) and is_valid_password(account['password'] and is_valid_username(account['username'])):
        try:
            r = connect_go_server('signin', account)
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "Errore: formato email e/o password non validi"