import pygame
import random

# Inicialización de Pygame
pygame.init()

# Dimensiones de la pantalla
screen_width = 800
screen_height = 600
screen = pygame.display.set_mode((screen_width, screen_height))

# Título del juego
pygame.display.set_caption("Space Coders")

# Colores
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
GREEN = (0, 255, 0)
BLUE = (0, 0, 255)

# Jugador
player_width = 50
player_height = 50
player_x = screen_width // 2 - player_width // 2
player_y = screen_height - player_height - 10
player_x_change = 0
player_y_change = 0

# Balas
bullet_radius = 8
bullet_x = 0
bullet_y = player_y
bullet_y_change = 3
bullet_state = "ready"  # "ready" significa que la bala está lista para disparar

# Enemigos
enemy_width = 50
enemy_height = 50
enemy_speed = 0.2
enemies = []

# Puntuación
score = 0
font = pygame.font.Font(None, 36)

# Función para dibujar la nave del jugador (cuadrado verde)
def player(x, y):
    pygame.draw.rect(screen, GREEN, (x, y, player_width, player_height))

# Función para disparar una bala
def fire_bullet(x, y):
    global bullet_state
    bullet_state = "fire"
    pygame.draw.circle(screen, BLUE, (x + player_width // 2, y), bullet_radius)
    pygame.draw.circle(screen, BLUE, (x + (player_width // 2) * 3, y), bullet_radius)
    pygame.draw.circle(screen, BLUE, (x - player_width // 2, y), bullet_radius)

# Función para generar un enemigo
def create_enemy():
    enemy_x = random.randint(0, screen_width - enemy_width)
    enemy_y = random.randint(-100, -40)
    enemies.append([enemy_x, enemy_y])

# Función para mover y dibujar los enemigos
def move_enemies():
    for enemy in enemies:
        enemy[1] += enemy_speed
        pygame.draw.rect(screen, RED, (enemy[0], enemy[1], enemy_width, enemy_height))

# Función para detectar colisiones
def is_collision(x1, y1, x2, y2, width, height):
    return x1 < x2 + width and x1 + player_width > x2 and y1 < y2 + height and y1 + player_height > y2

# Función para mostrar la puntuación
def show_score(x, y, score):
    score_text = font.render("Score: " + str(score), True, WHITE)
    screen.blit(score_text, (x, y))

# Bucle principal del juego
running = True
while running:

    # Color de fondo
    screen.fill(BLACK)

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

        # Comprobación de pulsación de teclas
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_LEFT:
                player_x_change = -0.5
            if event.key == pygame.K_RIGHT:
                player_x_change = 0.5
            if event.key == pygame.K_UP:
                player_y_change = -0.5
            if event.key == pygame.K_DOWN:
                player_y_change = 0.5
            if event.key == pygame.K_SPACE:
                #if bullet_state == "ready":
                bullet_x = player_x
                fire_bullet(bullet_x, bullet_y)

        if event.type == pygame.KEYUP:
            if event.key in (pygame.K_LEFT, pygame.K_RIGHT):
                player_x_change = 0
            if event.key in (pygame.K_UP, pygame.K_DOWN):
                player_y_change = 0

    # Movimiento del jugador
    player_x += player_x_change
    player_y += player_y_change

    # Límites para el movimiento del jugador
    if player_x <= 0:
        player_x = 0
    elif player_x >= screen_width - player_width:
        player_x = screen_width - player_width

    if player_y <= 0:
        player_y = 0
    elif player_y >= screen_height - player_height:
        player_y = screen_height - player_height

    # Movimiento de la bala
    if bullet_state == "fire":
        fire_bullet(bullet_x, bullet_y)
        bullet_y -= bullet_y_change

    # Verificar si la bala se sale de la pantalla
    if bullet_y <= 0:
        bullet_y = player_y
        bullet_state = "ready"

    # Generar nuevos enemigos
    if random.randint(0, 100) < 5:
        create_enemy()

    # Mover los enemigos
    move_enemies()

    # Verificar colisiones entre bala y enemigos
    for enemy in enemies:
        if is_collision(enemy[0], enemy[1], bullet_x + player_width // 2 - bullet_radius, bullet_y, enemy_width, enemy_height):
            bullet_y = player_y
            bullet_state = "ready"
            enemies.remove(enemy)
            score += 1

        # Verificar colisiones entre jugador y enemigos
        if is_collision(player_x, player_y, enemy[0], enemy[1], enemy_width, enemy_height):
            running = False  # Termina el juego si hay colisión

    # Llamada a las funciones para dibujar al jugador y mostrar la puntuación
    player(player_x, player_y)
    show_score(10, 10, score)

    # Actualizar la pantalla
    pygame.display.update()

# Salir de Pygame
pygame.quit()
